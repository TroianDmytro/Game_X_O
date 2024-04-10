
namespace MyUsers
{
    public class Room
    {
        public List<MyUsers.User> UsersList;

        public FieldGame Field {  get; set; }

        MyUsers.User User1 { get; set; }
        MyUsers.User User2 { get; set; }
        public Room()
        {
            UsersList = new List<MyUsers.User>();
            //CreateUsersList();
            Field = new FieldGame();
        }

        public void CreateUsersList()
        {
            UsersList = [];
            UsersList.Add(new MyUsers.User());
            UsersList.Add(new MyUsers.User());
        }

        public bool IsFullUsersList()
        {
            foreach(MyUsers.User user in this.UsersList)
            {
                if (user == null || user.Socket == null)
                {
                    return false;
                }
            }
            return true;
        }
        public bool AddToRoom(MyUsers.User user)
        {
            if (UsersList.Count < 2)
            {
                UsersList.Add(user);
                return true;
            }
            return false;
        }

    }
}
