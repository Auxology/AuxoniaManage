using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Auth.Verify_Email;

public sealed record VerifyEmailCommand
(
    string UserId,
    string Token
) : ICommand<VerifyEmailResponse>;

public sealed record VerifyEmailResponse 
(
    string UserId,
    DateTime VerifiedAt
);