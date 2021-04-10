using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WASM_Client.Services
{
    public class AppState
    {
        public event Action OnLoggedInChange;
        public event Action OnLoggingInChange;

        public string UserName { get; set; }
        public string Password { get; set; }

        public void UpdatingLoggedInState()
        {
            NotifyLoggedInStateChanged();
        }

        public void LoggingIn(string username, string password)
        {
            UserName = username;
            Password = password;
            NotifyLoggingIn();
        }

        private void NotifyLoggedInStateChanged() => OnLoggedInChange?.Invoke();

        private void NotifyLoggingIn() => OnLoggingInChange?.Invoke();
    }
}

