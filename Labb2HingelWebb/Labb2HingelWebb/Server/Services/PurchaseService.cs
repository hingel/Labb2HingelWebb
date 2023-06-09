﻿using Labb2HingelWebb.Shared;
using Labb2HingelWebb.Shared.DTOs;

namespace Labb2HingelWebb.Server.Services;

public class PurchaseService
{
	public async Task<ServiceResponse<string>> PlaceOrder(OrderDto newOrderDto)
	{
		//Betalningssimulering
		var randomPayment = new Random();

		if (randomPayment.Next(0, 11) < 7)
		{
			return new ServiceResponse<string>()
			{
				Message = "Order sent. Thank you!",
				Success = true
			};
		}

		return new ServiceResponse<string>()
		{
			Message = "Payment failed",
			Success = false
		};
	}
}