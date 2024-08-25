namespace TodoProject.models
{
    public class TodoDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string UsersCollectionName { get; set; }
        public string CardsCollectionName { get; set; }
        public string LabelsCollectionName { get; set; }
        public string CardLabelCollectionName { get; set; }
        public string CommentCollectionName { get; set; }
        public string RolesCollectionName { get; set; }
    }
}
