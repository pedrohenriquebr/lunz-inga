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
using LuzInga.Application.Common.CQRS;
using ContactEntity = LuzInga.Domain.Entities.Contact;
namespace LuzInga.Application.Usecases.Contact.AddContact;

public class AddContactHandler
    : ActionHandler<AddContactRequest, ActionResult<AddContactResponse>>
{
    private readonly ILuzIngaContext context;
    private readonly IBloomFilter bloomFilter;

    public AddContactHandler(ILuzIngaContext context,  IBloomFilter bloomFilter)
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

            entity = ContactEntity.Create(request.Email,request.Name);
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
            new AddContactResponse() { ContactId = entity?.Id ?? 0 }
        );
    }
}

public sealed class AddContactRequest
{
    public string Email { get; set; }
    public string Name { get; set; }
}


public sealed class AddContactResponse
{
    public int ContactId { get; set; }
}
