using WASM_Client.Models;
using WASM_Client.Models.Account;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Net.Http.Json;

namespace WASM_Client.Services
{
    public interface IAccountService
    {
        UserModel User { get; }
        Task Initialize();
        Task Login(Login model);
        Task Logout();
        Task SetUser(UserModel user);
        //Task Register(AddUser model);
        //Task<IList<UserModel>> GetAll();
        //Task<UserModel> GetById(int id);
        // Task Update(int id, EditUser model);
        // Task Delete(int id);
    }

    public class AccountService : IAccountService, IDisposable
    {
        private NavigationManager _navigationManager;
        private ILocalStorageService _localStorageService;
        private HttpClient _httpClient;
        private string _userKey = "user";
        private AppState _appState;

        public UserModel User { get; private set; }

        public AccountService(
            HttpClient httpClient,
            AppState appState,
            NavigationManager navigationManager,
            ILocalStorageService localStorageService) 
        
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
            _appState = appState;

            _appState.OnLoggingInChange += DoLogin;
          }

        public async Task SetUser(UserModel user)
        {
            User = user;
            await _localStorageService.SetItem(_userKey, User);
        }

        public async void DoLogin()
        {
            Login model = new()
            {
                username = _appState.UserName,
                password = _appState.Password
            };
            await Login(model);
        }

        public async Task Initialize()
        {
            User = await _localStorageService.GetItem<UserModel>(_userKey);
        }

        public async Task Login(Login model)
        {
            var response = await _httpClient.PostAsJsonAsync($"account/login", model);
   
            if (response.IsSuccessStatusCode)
            {
                User = await response.Content.ReadFromJsonAsync<UserModel>();
                await _localStorageService.SetItem(_userKey, User);
                _appState.UpdatingLoggedInState();
                _navigationManager.NavigateTo("/matches");
            }
        }

        public async Task Logout()
        {
            User = null;
            await _localStorageService.RemoveItem(_userKey);
            _navigationManager.NavigateTo("/");
        }

        public void Dispose()
        {
            _appState.OnLoggingInChange -= DoLogin;
        }

        //public async Task Register(AddUser model)
        //{
        //    await _httpService.Post("/users/register", model);
        //}

        //public async Task<IList<UserModel>> GetAll()
        //{
        //    return await _httpService.Get<IList<UserModel>>("/users");
        //}

        //public async Task<UserModel> GetById(int id)
        //{
        //    return await _httpService.Get<UserModel>($"/users/{id}");
        //}

        //public async Task Update(int id, EditUser model)
        //{
        //    await _httpService.Put($"/users/{id}", model);

        //    // update stored user if the logged in user updated their own record
        //    if (id == User.Id) 
        //    {
        //        // update local storage
        //        User.username = model.Username;
        //        await _localStorageService.SetItem(_userKey, User);
        //    }
        //}

        //public async Task Delete(int id)
        //{
        //    await _httpService.Delete($"/users/{id}");

        //    // auto logout if the logged in user deleted their own record
        //    if (id == User.Id)
        //        await Logout();
        //}
    }
}