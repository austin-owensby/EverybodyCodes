namespace EverybodyCodes.Gateways
{
    public class EverybodyCodesGateway
    {
        private HttpClient? client;
        private readonly int throttleInMinutes = 3;
        private DateTimeOffset? lastCall = null;

        /// <summary>
        /// For a given year and quest, get the user's puzzle input
        /// </summary>
        /// <param name="year"></param>
        /// <param name="quest"></param>
        /// <returns></returns>
        public async Task<string> ImportInput(int year, int quest)
        {
            ThrottleCall();

            throw new NotImplementedException();

            // TODO, need to figure out how to implement this

            /*
            HttpRequestMessage message = new(HttpMethod.Get, $"/{year}/quest/{quest}/input");

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

            HttpResponseMessage result = await client!.SendAsync(message);
            string response = await GetSuccessfulResponseContent(result);

            return response;
            */
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

            Dictionary<string, string> data = new()
            {
                { "answer", answer }
            };

            HttpContent request = new FormUrlEncodedContent(data);

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

            string response = await GetSuccessfulResponseContent(result);

            // TODO parse response

            return response;
        }

        /// <summary>
        /// Ensure that the response was successful and return the parsed response if it was
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private static async Task<string> GetSuccessfulResponseContent(HttpResponseMessage result)
        {
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadAsStringAsync();
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
    }
}