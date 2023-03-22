using System.IO.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using LuzInga.Application.Services;

namespace LuzInga.Application.Usecases.Contact.AddContact;

public class AddContactHandler
    : EndpointBaseAsync.WithRequest<AddContactRequest>.WithActionResult<AddContactResponse>
{
    private readonly IDbContext context;
    private readonly IBloomFilter bloomFilter;

    public AddContactHandler(IDbContext context,  IBloomFilter bloomFilter)
    {
        this.context = context;
        this.bloomFilter = bloomFilter;
    }

    [HttpPost("/api/contact")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [SwaggerOperation(
        Summary = "AddContact",
        OperationId = "Contact.AddContact",
        Tags = new[] { "Contact" }
    )]
    public override async Task<ActionResult<AddContactResponse>> HandleAsync(
        [FromBody] AddContactRequest request,
        CancellationToken cancellationToken = default
    )
    {
        Domain.Entities.Contact? entity = null;

        try
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Name))
                return BadRequest();

            entity = new Domain.Entities.Contact(email: request.Email, name: request.Name);

            await context.Contact.AddAsync(entity);
            await context.SaveChangesAsync();
            bloomFilter.Add(entity.Email);
        }
        catch (Exception ex) { 
            return StatusCode(StatusCodes.Status500InternalServerError, new {
                error = ex.Message,
                details = ex.StackTrace
            });
        }

        return StatusCode(
            StatusCodes.Status201Created,
            new AddContactResponse() { ContactId = entity?.ContactId ?? 0 }
        );
    }
}
