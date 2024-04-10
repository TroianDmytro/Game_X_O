using System.Text.Json;

namespace Cours
{
    public class PlayerCours
    {
        public string PlayerLogin {  get; set; }
        public int PlayerIndex { get; set; }
        public int Index_Y { get; set; }
        public int Index_X { get; set; }
        public char PlayerSimbol { get; set; }

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

        public string WriteToJSON()
        {
            if (this != null)
            {
                return JsonSerializer.Serialize(this);
            }
            return string.Empty;
        }

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
