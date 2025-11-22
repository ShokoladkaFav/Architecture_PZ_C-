using MediatR;
using System;
using System.Collections.Generic;

namespace Application.Features.Orders.Queries.GetOrdersByUserId
{
    public class GetOrdersByUserIdQuery : IRequest<List<OrderListDto>>
    {
        public Guid UserId { get; set; }
    }
}