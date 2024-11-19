using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace EverybodyCodes.Gateways
{
    public class EverybodyCodesGateway
    {
        private HttpClient? client;
        private HttpClient? cdnClient;
        private readonly int throttleInMinutes = 3;
        private DateTimeOffset? lastCall = null;
        private readonly JsonSerializerOptions jsonSerializerOptions = new() {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private int? seed;

        /// <summary>
        /// For a given year, quest, and part, get the user's puzzle input
        /// </summary>
        /// <param name="year"></param>
        /// <param name="quest"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        public async Task<string> ImportInput(int year, int quest, int part)
        {
            ThrottleCall();
            
            string cipherText = await GetInputCipher(year, quest, part);
            string key = await GetInputKey(year, quest, part);

            string output = DecryptPuzzleInput(cipherText, key);

            return output;
        }

        /// <summary>
        /// Get the input key for a specific part
        /// </summary>
        /// <param name="year"></param>
        /// <param name="quest"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        private async Task<string> GetInputKey(int year, int quest, int part) {
            HttpRequestMessage message = new(HttpMethod.Get, $"/api/event/{year}/quest/{quest}");

            if (client == null)
            {
                InitializeClient();
            }

            HttpResponseMessage result = await client!.SendAsync(message);
            QuestResponse? response = await GetSuccessfulResponseContent<QuestResponse>(result);

            if (response == null) {
                throw new Exception("Unable to parse the input key");
            }

            string? key = part switch {
                1 => response.Key1,
                2 => response.Key2,
                3 => response.Key3,
                _ => throw new Exception($"Unknown part")
            };

            if (key == null) {
                throw new Exception("Could not find a key for this part, make sure that you completed the previous part for this quest");
            }
            
            return key;
        }

        /// <summary>
        /// Get the cipher text for the given quest and part
        /// </summary>
        /// <param name="year"></param>
        /// <param name="quest"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        private async Task<string> GetInputCipher(int year, int quest, int part) {
            InputResponse? response;

            string cipherPath = Path.Combine(Environment.CurrentDirectory, $"Inputs/{year}/{quest:D2}/cipher.json");
            
            if (File.Exists(cipherPath)) {
                string cipherJson = File.ReadAllText(cipherPath);
                response = JsonSerializer.Deserialize<InputResponse>(cipherJson);
                
                if (response == null) {
                    throw new Exception("Unable to parse the input from the cached input");
                }
            }
            else {
                if (seed == null) {
                    seed = await GetSeed();
                }

                HttpRequestMessage message = new(HttpMethod.Get, $"/assets/{year}/{quest}/input/{seed}.json");

                if (cdnClient == null)
                {
                    InitializeCDNClient();
                }

                HttpResponseMessage result = await cdnClient!.SendAsync(message);
                response = await GetSuccessfulResponseContent<InputResponse>(result);
                
                if (response == null) {
                    throw new Exception("Unable to parse the input from the CDN");
                }

                // Cache the input so that we don't have to query it on all 3 parts of the quest
                string questFolderPath = Path.Combine(Environment.CurrentDirectory, $"Inputs/{year}/{quest:D2}");
 
                if (!Directory.Exists(questFolderPath))
                {
                    Directory.CreateDirectory(questFolderPath);
                }

                using StreamWriter inputFile = new(cipherPath);
                string cipherJson = JsonSerializer.Serialize(response);
                await inputFile.WriteAsync(cipherJson);
            }

            string cipherText = part switch {
                1 => response.PartOne,
                2 => response.PartTwo,
                3 => response.PartThree,
                _ => throw new Exception($"Unknown part")
            };
            
            return cipherText;
        }

        /// <summary>
        /// Get's the logged in user's seed. Caches it for later use once retrieved
        /// </summary>
        /// <returns></returns>
        private async Task<int> GetSeed() {
            string seedPath = Path.Combine(Environment.CurrentDirectory, $"Gateways/Seed.txt");

            if (File.Exists(seedPath)) {
                string seedData = File.ReadAllText(seedPath);
                if (int.TryParse(seedData, out int parsedSeed)) {
                    return parsedSeed;
                }
            }
            
            HttpRequestMessage message = new(HttpMethod.Get, "/api/user/me");

            if (client == null)
            {
                InitializeClient();
            }

            HttpResponseMessage result = await client!.SendAsync(message);
            ProfileResponse? response = await GetSuccessfulResponseContent<ProfileResponse>(result);

            if (response == null) {
                throw new Exception("Unable to parse profile api response");
            }

            if (response.Seed == 0) {
                throw new Exception("Your session has expired, please update your Cookie.txt. See the ReadMe for more.");
            }

            if (!File.Exists(seedPath)) {
                using StreamWriter inputFile = new(seedPath);
                await inputFile.WriteAsync($"{response.Seed}");
            }

            return response.Seed;
        }

        /// <summary>
        /// Decrypt the puzzle input, specific to Everybody Codes
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string DecryptPuzzleInput(string cipherText, string key) {
            // 1) Replace 21st character of key with ~
            char[] keyArray = key.ToCharArray();
            keyArray[20] = '~';
            key = new(keyArray);

            // 2) Set initialization vector from the first 16 chars of the key
            string iv = key[..16];

            // 3) Convert HEX string of encrypted text to byte array
            byte[] encryptedTextBytes = Convert.FromHexString(cipherText);

            // 4) Convert UTF8 key and iv to byte array
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);

            // 5) Use AES to decrypt the input into plain text
            string output = DecryptStringFromBytes_Aes(encryptedTextBytes, keyBytes, ivBytes);

            return output;
        }

        /// <summary>
        /// Built in C# AES Decryption, example take from https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=net-8.0
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        private static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] key, byte[] iv)
        {    
            // CBC
            // Pkcs7

            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(nameof(cipherText));
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException(nameof(iv));

            // Declare the string used to hold
            // the decrypted text.
            string? plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new(cipherText))
                {
                    using (CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        /// <summary>
        /// Send the user's answer to the specific question
        /// </summary>
        /// <param name="year"></param>
        /// <param name="quest"></param>
        /// <param name="part"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public async Task<string> SubmitAnswer(int year, int quest, int part, string answer)
        {
            ThrottleCall();

            SubmitAnswerRequest data = new(){Answer = answer};

            string stringData = JsonSerializer.Serialize(data, jsonSerializerOptions);
           
            HttpContent request = new StringContent(stringData, Encoding.UTF8, "application/json");

            if (client == null)
            {
                try
                {
                    InitializeClient();
                }
                catch
                {
                    return "Unable to read Cookie.txt. Make sure that it exists in the PuzzleHelper folder. See the ReadMe for more.";
                }
            }

            HttpResponseMessage result = await client!.PostAsync($"/api/event/{year}/quest/{quest}/part/{part}/answer", request);

            string stringResponse = await GetSuccessfulResponseContentForSubmit(result);

            return stringResponse;
        }

        /// <summary>
        /// Ensure that the response was successful and return the parsed response if it was
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private static async Task<string> GetSuccessfulResponseContentForSubmit(HttpResponseMessage result)
        {
            if (result.StatusCode == HttpStatusCode.Conflict) {
                return "This quest and part has already been submitted";
            }

            if ((int)result.StatusCode == 418) {
                return "Your Cookie has expired, please update your Cookie.txt file. See the ReadMe for more info.";
            }

            result.EnsureSuccessStatusCode();

            SubmitAnswerResponse? data = await result.Content.ReadFromJsonAsync<SubmitAnswerResponse>();

            return data != null && data.Correct ? "Correct" : "Incorrect";
        }

        /// <summary>
        /// Ensure that the response was successful and return the parsed response if it was
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private static async Task<T?> GetSuccessfulResponseContent<T>(HttpResponseMessage result)
        {
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadFromJsonAsync<T>();
        }

        /// <summary>
        /// Tracks the last API call and prevents another call from being made until after the configured limit
        /// </summary>
        private void ThrottleCall()
        {
            // If someone is running the project for the first time that's 400 calls
            if (lastCall != null && (DateTimeOffset.Now < lastCall.Value.AddMinutes(throttleInMinutes)))
            {
                throw new Exception($"Unable to make another API call to Everybody Codes Server because we are attempting to throttle calls according to their specifications (See more in the ReadMe). Please try again after {lastCall.Value.AddMinutes(throttleInMinutes)}.");
            }
            else
            {
                lastCall = DateTimeOffset.Now;
            }
        }

        /// <summary>
        /// Initialize the Http Client using the user's cookie
        /// </summary>
        private void InitializeClient()
        {
            // We're waiting to do this until the last moment in case someone want's to use the code base without setting up the cookie
            client = new HttpClient
            {
                BaseAddress = new Uri("https://everybody.codes/")
            };

            client.DefaultRequestHeaders.UserAgent.ParseAdd(".NET 8.0 (+via https://github.com/austin-owensby/EverybodyCodes by austin_owensby@hotmail.com)");

            string[] fileData;

            try
            {
                fileData = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "PuzzleHelper/Cookie.txt"));
            }
            catch (Exception)
            {
                throw new Exception("Unable to read Cookie.txt. Make sure that it exists in the PuzzleHelper folder. See the ReadMe for more.");
            }
            
            if (fileData.Length == 0 || string.IsNullOrWhiteSpace(fileData[0])) {
                throw new Exception("Cookie.txt is empty. Please ensure it is properly populated and saved. See the ReadMe for more.");
            }
            if (fileData.Length > 1) {
                throw new Exception("Detected multiple lines in Cookie.txt, ensure that the whole cookie is on 1 line.");
            }

            string cookie = fileData[0];
            client.DefaultRequestHeaders.Add("Cookie", cookie);
        }

        /// <summary>
        /// Initialize the CDN Http Client
        /// </summary>
        private void InitializeCDNClient()
        {
            // We're waiting to do this until the last moment in case someone want's to use the code base without setting up the cookie
            cdnClient = new HttpClient
            {
                BaseAddress = new Uri("https://everybody-codes.b-cdn.net/")
            };

            cdnClient.DefaultRequestHeaders.UserAgent.ParseAdd(".NET 8.0 (+via https://github.com/austin-owensby/EverybodyCodes by austin_owensby@hotmail.com)");
        }
    }
}