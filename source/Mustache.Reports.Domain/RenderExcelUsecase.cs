﻿using Mustache.Reports.Boundry;
using Mustache.Reports.Boundry.Report.Word;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;

namespace Mustache.Reports.Domain
{
    public class RenderWordUseCase : IRenderWordUseCase
    {
        private readonly IWordGateway _wordGateway;

        public RenderWordUseCase(IWordGateway wordGateway)
        {
            _wordGateway = wordGateway;
        }

        public void Execute(RenderWordInput inputInput, IRespondWithSuccessOrError<IFileOutput, ErrorOutputMessage> presenter)
        {
            var result = _wordGateway.CreateReport(inputInput);

            if (result.HasErrors())
            {
                RespondWithErrors(presenter, result);
                return;
            }

            RespondWithFile(inputInput, presenter, result);
        }

        private static void RespondWithFile(RenderWordInput inputInput, IRespondWithSuccessOrError<IFileOutput, ErrorOutputMessage> presenter, RenderedDocummentOutput result)
        {
            var reportMessage = new WordFileOutput(inputInput.ReportName, result.FetchDocumentAsByteArray());

            presenter.Respond(reportMessage);
        }

        private void RespondWithErrors(IRespondWithSuccessOrError<IFileOutput, ErrorOutputMessage> presenter, RenderedDocummentOutput result)
        {
            var errors = new ErrorOutputMessage();
            errors.AddErrors(result.ErrorMessages);
            presenter.Respond(errors);
        }
    }
}