using DTicks.Views;

namespace DTicks
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(TaskDetailPage), typeof(TaskDetailPage));
            Routing.RegisterRoute(nameof(TaskModifyPage), typeof(TaskModifyPage));
            Routing.RegisterRoute(nameof(NewTaskPage), typeof(NewTaskPage));
        }
    }
}
