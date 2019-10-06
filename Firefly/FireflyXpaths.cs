namespace Firefly
{
    public class FireflyXpaths
    {
        public string usernamebox = "//input[@class='ff-login-user ff-login-input']";
        public string passwordbox = "//input[@class='ff-login-password ff-login-input']";
        public string loginbtn = "//button[@class='ff-login-submit']";
        public string newtaskbtn = "//span[@data-testid='set-a-new-task']";
        public string alltasksbtn = "//span[contains(text(),'Tasks')]";
        public string tasksTable = "//div[@data-testid='task-list']";
        public string taskOverview = "//a[@href='#task-overview']";
        public string taskEdit = "//button[@data-testid='edit']";
        public string taskEditTitle = "//input[@name='task.title']";
        public string taskEditSave = "//button[@data-testid='save-changes']";
        public string taskEditView = "//a[@data-testid='view-task']";
        public string taskTitle = "//h1[@data-testid='page-header-title']";


    }
}