﻿using Core.Hotels.Rooms;

namespace HotelReservationWebsite.Hotels.Rooms
{
    public static class RoomExtensions
    {
        public static RoomDto ToDto(this Room room)
        {
            if (room == null)
                return null;

            return new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
                Description = room.Description,
                RoomSize = room.RoomSize,
                Price = room.Price
            };
        }
    }
}