﻿using Core.Hotels;
using FluentAssertions;
using HotelReservationWebsite.Hotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Core.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using Core.User;
using System.Net.Http.Headers;
using Azure;
using Application.Users;
using Application.Hotels.Rooms.AddReservation;
using Castle.Core.Resource;
using Core.Entities.Rooms;

namespace HotelReservation.Tests
{
    public class ReservationTests : BaseIntegrationTest
    {
        public ReservationTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task MakeReservation_ValidReservation_Success()
        {
            var hotel = new Hotel()
            {
                Name = "Test",
                Description = "Test",
                Address = new Address() { },
                Rooms = new List<Room>
                {
                    new Room {
                        Id = 1,
                        Name = "Test",
                        Description = "Test",
                        Price = 123,
                        RoomSize = 10
                    }
                }
            };

            var user = new AppUser()
            {
                Id = Guid.NewGuid().ToString(),
                DisplayName = "Test",
                Email = "Test@test.com",
                Password = "Pa$$w0rd"
            };

            var loginDto = new LoginDto
            {
                Email = "Test@test.com",
                Password = "Pa$$w0rd"
            };

            var reservation = new Reservation()
            {
                RoomId = 1,
                Begin = DateTime.UtcNow.AddHours(1),
                End = DateTime.UtcNow.AddHours(2),
                CustomerId = Guid.Parse(user.Id)
            };

            AddReservationCommand command = new AddReservationCommand(1, Guid.Parse(user.Id), reservation);

            HttpResponseMessage postUserResponse = await HttpClient.PostAsJsonAsync("api/Account/register", user);

            Assert.True(postUserResponse.IsSuccessStatusCode);

            HttpResponseMessage loginResponse = await HttpClient.PostAsJsonAsync("api/Account/login", loginDto);

            Assert.True(loginResponse.IsSuccessStatusCode);

            var userDto = await loginResponse.Content.ReadFromJsonAsync<UserDto>();

            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userDto.Token);

            HttpResponseMessage postRoomResponse = await HttpClient.PostAsJsonAsync("api/Hotel", hotel);

            HttpResponseMessage postReservationResponse = await HttpClient.PostAsJsonAsync("api/Room/1/reservations", reservation);

            Assert.True(postReservationResponse.IsSuccessStatusCode);
        }
    }
}