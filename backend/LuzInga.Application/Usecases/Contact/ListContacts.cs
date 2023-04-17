using AspNetCore.IQueryable.Extensions;
using AspNetCore.IQueryable.Extensions.Attributes;
using AspNetCore.IQueryable.Extensions.Filter;
using AspNetCore.IQueryable.Extensions.Pagination;
using AspNetCore.IQueryable.Extensions.Sort;
using LuzInga.Application.Common;
using LuzInga.Application.Common.CQRS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using ContactEntity = LuzInga.Domain.Entities.Contact;

namespace LuzInga.Application.Usecases.Contact.ListContacts;

public sealed class ListContactsHandler : QueryHandler<ListContactsRequest, PaginatedResponse<ContactEntity>>
{
    private readonly ILuzIngaContext context;

    public ListContactsHandler(ILuzIngaContext context)
    {
        this.context = context;
    }

    [HttpGet("/api/contact")]
    [SwaggerOperation(
        Summary = "Paginate and filter all contacts",
        Description = "Return a page of contacts",
        OperationId = "Contact.ListContacts",
        Tags = new[] { "Contact" }
    )]
    public override async Task<PaginatedResponse<ContactEntity>> HandleAsync(
        [FromQuery]
        ListContactsRequest request,
        CancellationToken cancellationToken = default)
    {
        return await PaginationFactory.Create(context.Contact, request);
    }
}


public sealed class ListContactsRequest : BasePaginated
{
    public int? ContactId { get; set; }

    [QueryOperator(Operator = WhereOperator.StartsWith, CaseSensitive = false)]
    public string? Email { get; set; }

    [QueryOperator(Operator = WhereOperator.StartsWith, CaseSensitive = false)]
    public string? Name { get; set; }
}