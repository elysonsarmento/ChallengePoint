using Asp.Versioning;
using AutoMapper;
using ChallengePoint.Application.ViewModel;
using ChallengePoint.Domain.DTO;
using ChallengePoint.Domain.Interface;
using ChallengePoint.Domain.Models;
using ChallengePoint.Exceptions;
using ChallengePoint.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ChallengePoint.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CollaboratorController(ICollaboratorRepository collaboratorRepository, IMapper mapper) : ControllerBase
    {
        private readonly ICollaboratorRepository _collaboratorRepository = collaboratorRepository;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Retrieves a paginated list of collaborators.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name = "pageQuantity">The number of items per page.</param>
        /// <returns>A paginated list of collaborators.</returns>
        [HttpGet]
        public async Task<ActionResult<SimplePagination<CollaboratorDto>>> GetAll(int pageNumber = 1, int pageQuantity = 10)
        {
            try
            {
                var allCollaborators = await _collaboratorRepository.GetAllAsync(pageNumber, pageQuantity);

                if (allCollaborators == null || !allCollaborators.Any())
                {
                    return NotFound("No collaborators found.");
                }

                var collaboratorsDTO = _mapper.Map<IEnumerable<CollaboratorDto>>(allCollaborators);

                var paginatedCollaborators = SimplePagination<CollaboratorDto>.Create(collaboratorsDTO, pageNumber, pageQuantity);

                return Ok(paginatedCollaborators);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException("Ocorreu um erro ao processar sua solicitação.", ex));
            }
        }

        /// <summary>
        /// Retrieves a collaborator by ID.
        /// </summary>
        /// <param name="id">The ID of the collaborator.</param>
        /// <returns>The collaborator with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CollaboratorDto>> GetById(int id)
        {
            try
            {
                var collaborator = await _collaboratorRepository.GetByIdAsync(id);
                if (collaborator == null)
                {
                    return NotFound($"Collaborator with ID {id} not found.");
                }
                return Ok(collaborator);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException("Ocorreu um erro ao processar sua solicitação.", ex));
            }
        }

        /// <summary>
        /// Adds a new collaborator.
        /// </summary>
        /// <param name="collaborator">The collaborator to add.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        public async Task<ActionResult<CollaboratorDto>> Add([FromBody] CollaboratorViewModel collaborator)
        {
            if (collaborator == null)
            {
                return BadRequest("Collaborator data is required.");
            }

            try
            {
                // Verificar se já existe um colaborador com a mesma matrícula
                if (await _collaboratorRepository.ExistsByEnrollmentAsync(collaborator.Enrollment))
                {
                    return BadRequest("Já existe um colaborador com essa matrícula.");
                }

                CollaboratorModel _collaborator = new() { Name = collaborator.Name, Position = collaborator.Position, Salary = collaborator.Salary, Enrollment = collaborator.Enrollment };
                await _collaboratorRepository.AddAsync(_collaborator);
                return CreatedAtAction(nameof(GetById), new { id = _collaborator.Id }, collaborator);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException("Ocorreu um erro ao processar sua solicitação.", ex));
            }
        }

        /// <summary>
        /// Updates an existing collaborator.
        /// </summary>
        /// <param name="id">The ID of the collaborator to update.</param>
        /// <param name="collaborator">The updated collaborator data.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CollaboratorViewModel collaborator)
        {
            if (collaborator == null)
            {
                return BadRequest("Collaborator ID mismatch or missing data.");
            }

            try
            {
                var existingCollaborator = await _collaboratorRepository.GetByIdAsync(id);
                if (existingCollaborator == null)
                {
                    return NotFound($"Collaborator with ID {id} not found.");
                }

                // Verificar se a matrícula foi alterada e se a nova matrícula já existe para outro colaborador
                if (existingCollaborator.Enrollment != collaborator.Enrollment &&
                    await _collaboratorRepository.ExistsByEnrollmentAsync(collaborator.Enrollment))
                {
                    return BadRequest("Já existe um colaborador com essa matrícula.");
                }

                var _collaborator = new CollaboratorModel { Id = id, Name = collaborator.Name, Position = collaborator.Position, Salary = collaborator.Salary, Enrollment = collaborator.Enrollment };

                await _collaboratorRepository.UpdateAsync(_collaborator);
                return NoContent();
            }
            catch (FormatException ex)
            {
                return BadRequest("Invalid ISO date/time format. Please use YYYY-MM-DDTHH:mm:ssZ or YYYY-MM-DDTHH:mm:ss±hh:mm");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException("Ocorreu um erro ao processar sua solicitação.", ex));
            }
        }

        /// <summary>
        /// Deletes a collaborator by ID.
        /// </summary>
        /// <param name="id">The ID of the collaborator to delete.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var collaborator = await _collaboratorRepository.GetByIdAsync(id);
                if (collaborator == null)
                {
                    return NotFound($"Collaborator with ID {id} not found.");
                }

                await _collaboratorRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (FormatException ex)
            {
                return BadRequest("Invalid ISO date/time format. Please use YYYY-MM-DDTHH:mm:ssZ or YYYY-MM-DDTHH:mm:ss±hh:mm");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException("Ocorreu um erro ao processar sua solicitação.", ex));
            }
        }
    }
}