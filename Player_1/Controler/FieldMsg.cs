
using System.Text.Json;

namespace Game.Controler
{
    public class FieldMsg
    {
        List<List<Button>> buttons;
        string _path = "Buttons.json";
        public FieldMsg()
        {
            buttons = [];
        }

        public FieldMsg(List<List<Button>> b)
        {
            buttons = b;
        }

        public void WriteToJSON()
        {
            if (buttons != null)
            {
                string temp = JsonSerializer.Serialize(buttons);

                using StreamWriter sw = new(_path, append: false);
                sw.Write(temp);
            }
        }

        public List<List<Button>> ReadWithJSON()
        {
            using StreamReader sr = new(_path);
            string line = sr.ReadLine();
            var tempObj = JsonSerializer.Deserialize<FieldMsg>(_path);
            buttons = tempObj.buttons;
            return buttons;
        }
    }
}
