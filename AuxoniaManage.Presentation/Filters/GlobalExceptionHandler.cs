using AuxoniaManage.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace AuxoniaManage.Presentation.Filters;

internal sealed class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = exception switch
        {
            // RFC 9110: 400 Bad Request - Client error due to malformed request
            InvalidCredentialsException => StatusCodes.Status400BadRequest,
            OldPasswordCannotBeSameAsNewPasswordException => StatusCodes.Status400BadRequest,
            NewEmailCannotBeSameAsOldEmailException => StatusCodes.Status400BadRequest,
            InvalidFileException => StatusCodes.Status400BadRequest,
            UnsupportedFileTypeException => StatusCodes.Status400BadRequest,
            FileSizeExceededException => StatusCodes.Status400BadRequest,
            InvalidInvitationTokenException => StatusCodes.Status400BadRequest,
            CannotAssignTaskToSelfException => StatusCodes.Status400BadRequest,
            InvalidProjectIdException => StatusCodes.Status400BadRequest,
            OneOrMoreAssigneesNotMemberOfWorkspaceException => StatusCodes.Status400BadRequest,
            
            // RFC 9110: 401 Unauthorized - Authentication required or failed
            EmailNotConfirmedException => StatusCodes.Status401Unauthorized,
            FailedToChangePasswordException => StatusCodes.Status401Unauthorized,
            
            // RFC 9110: 403 Forbidden - Authorization failed (authenticated but not authorized)
            WorkspaceAccessDeniedException => StatusCodes.Status403Forbidden,
            OnlyOwnerCanTransferOwnershipException => StatusCodes.Status403Forbidden,
            OnlyOwnerCanKickMemberException => StatusCodes.Status403Forbidden,
            TransferOwnershipFirstException => StatusCodes.Status403Forbidden,
            UserDoesNotHavePermissionException => StatusCodes.Status403Forbidden,
            OnlyWorkspaceOwnerCanUpdateException => StatusCodes.Status403Forbidden,
            OnlyOwnerCanRotateInvitationException => StatusCodes.Status403Forbidden,
            YouAreNotMemberOfWorkspaceException => StatusCodes.Status403Forbidden,
            InsufficientPermissionsException => StatusCodes.Status403Forbidden,
            LackOfPermissionException => StatusCodes.Status403Forbidden,
            CannotAssignRolesToHigherHierarchyException => StatusCodes.Status403Forbidden,
            
            // RFC 9110: 404 Not Found - Resource does not exist
            UserNotFoundException => StatusCodes.Status404NotFound,
            WorkspaceNotFoundException => StatusCodes.Status404NotFound,
            WorkspaceReadModelNotFoundException => StatusCodes.Status404NotFound,
            MembershipNotFoundException => StatusCodes.Status404NotFound,
            ProfileNotFoundException => StatusCodes.Status404NotFound,
            ProfileReadModelNotFoundException => StatusCodes.Status404NotFound,
            ProjectNotFoundException => StatusCodes.Status404NotFound,
            ProjectReadModelNotFoundException => StatusCodes.Status404NotFound,
            MembershipReadModelNotFoundException => StatusCodes.Status404NotFound,
            UserIsNotMemberException => StatusCodes.Status404NotFound,
            TaskNotFoundException => StatusCodes.Status404NotFound,
            CouldNotFindWorkspaceException => StatusCodes.Status404NotFound,
            CouldNotFindProjectException => StatusCodes.Status404NotFound,
            
            // RFC 9110: 409 Conflict - Resource state conflicts with request
            UserAlreadyExistsException => StatusCodes.Status409Conflict,
            WorkspaceNameAlreadyExistsException => StatusCodes.Status409Conflict,
            WorkspaceReadModelAlreadyExistsException => StatusCodes.Status409Conflict,
            MembershipAlreadyExistsException => StatusCodes.Status409Conflict,
            ProfileAlreadyExistsException => StatusCodes.Status409Conflict,
            ProfileReadModelAlreadyExistsException => StatusCodes.Status409Conflict,
            AlreadyAdminException => StatusCodes.Status409Conflict,
            ProjectReadModelAlreadyExistsException => StatusCodes.Status409Conflict,
            MembershipReadModelAlreadyExistsException => StatusCodes.Status409Conflict,
            
            // RFC 9110: 422 Unprocessable Content - Semantic errors in request
            PasswordResetFailedException => StatusCodes.Status422UnprocessableEntity,
            EmailVerificationFailedException => StatusCodes.Status422UnprocessableEntity,
            ChangeEmailRequestFailedException => StatusCodes.Status422UnprocessableEntity,
            WorkspaceOwnershipTransferFailedException => StatusCodes.Status422UnprocessableEntity,
            
            // RFC 9110: 423 Locked - Resource is locked/rate limited
            UserLockedOutException => StatusCodes.Status423Locked,
            
            // RFC 9110: 429 Too Many Requests - Rate limiting exceeded
            WorkspaceMemberLimitExceededException => StatusCodes.Status429TooManyRequests,
            
            // RFC 9110: 503 Service Unavailable - Service temporarily unavailable
            StorageServiceUnavailableException => StatusCodes.Status503ServiceUnavailable,
            
            // RFC 9110: 500 Internal Server Error - Server-side processing failures
            UserRegistrationFailedException => StatusCodes.Status500InternalServerError,
            WorkspaceCreationFailedException => StatusCodes.Status500InternalServerError,
            WorkspaceUpdateFailedException => StatusCodes.Status500InternalServerError,
            WorkspaceDeletionFailedException => StatusCodes.Status500InternalServerError,
            WorkspaceReadModelCreationFailedException => StatusCodes.Status500InternalServerError,
            WorkspaceReadModelUpdateFailedException => StatusCodes.Status500InternalServerError,
            WorkspaceReadModelDeletionFailedException => StatusCodes.Status500InternalServerError,
            MembershipCreationFailedException => StatusCodes.Status500InternalServerError,
            MembershipUpdateFailedException => StatusCodes.Status500InternalServerError,
            MembershipDeletionFailedException => StatusCodes.Status500InternalServerError,
            FailedToKickMemberException => StatusCodes.Status500InternalServerError,
            ProfileCreationFailedException => StatusCodes.Status500InternalServerError,
            ProfileUpdateFailedException => StatusCodes.Status500InternalServerError,
            ProfileReadModelCreationFailedException => StatusCodes.Status500InternalServerError,
            ProfileReadModelUpdateFailedException => StatusCodes.Status500InternalServerError,
            FileUploadFailedException => StatusCodes.Status500InternalServerError,
            FileDeletionFailedException => StatusCodes.Status500InternalServerError,
            FailedToCreateProjectException => StatusCodes.Status500InternalServerError,
            FailedToUpdateProjectException => StatusCodes.Status500InternalServerError,
            ProjectReadModelCreationFailedException => StatusCodes.Status500InternalServerError,
            ProjectReadModelUpdateFailedException => StatusCodes.Status500InternalServerError,
            FailedToCreateMembershipReadModelException => StatusCodes.Status500InternalServerError,
            FailedToDeleteMembershipReadModelException => StatusCodes.Status500InternalServerError,
            FailedToCreateTaskException => StatusCodes.Status500InternalServerError,
            FailedToEditTaskException => StatusCodes.Status500InternalServerError,
            CouldNotDeleteTaskException => StatusCodes.Status500InternalServerError,
            
            // Default fallback for any other ApplicationException
            ApplicationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
        
        var activity = httpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                
                Type = exception.GetType().Name,
                Title = "An error occurred",
                Detail = exception.Message,
                Instance = $"{httpContext.Request.Method}:{httpContext.Request.Path}",
                Extensions = new Dictionary<string, object?>
                {
                    {"requestId", httpContext.TraceIdentifier},
                    {"traceId", activity?.Id}
                }
            }
        });
    }
}