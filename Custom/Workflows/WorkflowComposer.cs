using adductio_umbraco.Custom.Workflows;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Forms.Core.Providers;

namespace adductio_umbraco.Custom.Startup;

public class WorkflowComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.WithCollectionBuilder<WorkflowCollectionBuilder>()
            .Add<MailingListWorkFlow>()
            ;
    }
}