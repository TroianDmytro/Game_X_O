using System.Text.Json;

namespace Cours
{
    public class PlayerCours
    {
        // логін
        public string PlayerLogin {  get; set; }
        // індекс гравця, 1 або 2
        public int PlayerIndex { get; set; }
        // координата ходу по У
        public int Index_Y { get; set; }
        // координата ходу по Х
        public int Index_X { get; set; }
        //символ який відображается на полі при ході
        public char PlayerSimbol { get; set; }
        // команда яка передается від сервера або від гравця на сервер
        public string ServerCommandLine { get; set; }
        public PlayerCours()
        {
            PlayerLogin = string.Empty;
            PlayerIndex = 0;
            Index_Y = 0;
            Index_X = 0;
            PlayerSimbol = '0';
            ServerCommandLine = string.Empty;
        }
        // записує хід в json формат
        public string WriteToJSON()
        {
            if (this != null)
            {
                return JsonSerializer.Serialize(this);
            }
            return string.Empty;
        }

        //считує хід з json формату
        public PlayerCours? ReadWithJSON(string strJSON)
        {
            var tempObj = JsonSerializer.Deserialize<PlayerCours>(strJSON);
            return tempObj;
        }

        public override string? ToString()
        {
            return $"{this.PlayerLogin}\t{this.PlayerIndex}\t{this.Index_Y}\t{this.Index_X}\t{this.PlayerSimbol}\t{this.ServerCommandLine}";
        }
    }
}
