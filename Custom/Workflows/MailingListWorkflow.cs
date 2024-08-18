using Umbraco.Forms.Core.Enums;
using Umbraco.Forms.Core.Persistence.Dtos;
using Umbraco.Forms.Core.Services;
using Umbraco.Forms.Core;

namespace adductio_umbraco.Custom.Workflows;

/// <summary>
/// Workflow that adds a form submission to a Mailing List form with the name and email fields.
/// It can then be exported and used for email marketing campaigns.
/// </summary>
public class MailingListWorkFlow : WorkflowType
{
    private readonly ILogger<MailingListWorkFlow> _logger;
    private readonly IRecordService _recordService;
    private readonly IFormService _formService;

    public MailingListWorkFlow(IRecordService recordService, IFormService formService, ILogger<MailingListWorkFlow> logger)
    {
        _logger = logger;
        _recordService = recordService ?? throw new ArgumentNullException(nameof(recordService));
        _formService = formService ?? throw new ArgumentNullException(nameof(formService));

        Id = new Guid("ccbeb0d5-adaa-4729-8b4c-4bb439dc0202");
        Name = "Mailing List Workflow";
        Description = "Adds a form submission to a Mailing List form.";
        Icon = "icon-chat-active";
        Group = "Services";
    }

    public override async Task<WorkflowExecutionStatus> ExecuteAsync(WorkflowExecutionContext context)
    {
        try
        {
            // Get the values from the form submission
            var values = context.Record.RecordFields.Values
                .ToDictionary(x => x.Alias, x => x.ValuesAsString());
            values.Add("sourceForm", context.Form.Name);

            // retrieve the a form named "Mailing List"
            var mailingListFormName = "Mailing List";
            var mailingListForm = _formService.Get(mailingListFormName);
            if (mailingListForm == null)
            {
                _logger.LogError("Target form with name {mailingListFormName} could not be found", mailingListFormName);
                return WorkflowExecutionStatus.Failed;
            }

            if (values["dataConsent"] != true.ToString()) {
                _logger.LogError("Data consent not granted");
                return WorkflowExecutionStatus.Failed;
            }

            if (values["name"] == null || values["email"] == null)
            {
                _logger.LogError("Name and Email fields are required for the Mailing List Workflow");
                return WorkflowExecutionStatus.Failed;
            }

            // Create a new record for the Mailing List form
            var record = new Record
            {
                Created = DateTime.Now,
                Form = mailingListForm.Id,
                Culture = Thread.CurrentThread.CurrentCulture.Name,
                RecordFields = mailingListForm.AllFields
                .Where(x => values.ContainsKey(x.Alias))
                .ToDictionary(field => field.Id, field => new RecordField(field)
                {
                    Alias = field.Alias,
                    FieldId = field.Id,
                    Key = Guid.NewGuid(),
                    Values = new List<object>() { values[field.Alias] } // retrieve the value from the form submission
                })
            };

            // Save the record to the target form
            await _recordService.SubmitAsync(record, mailingListForm);

            return WorkflowExecutionStatus.Completed;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while executing the Mailing List Workflow");
            return WorkflowExecutionStatus.Failed;
        }
    }

    public override List<Exception> ValidateSettings()
    {
        return new List<Exception>();
    }
}