
namespace DemoMVC.Models
{
    public class AutoGenerateCode
    {
        private int _nextIndex = 1;
        private string _prefix = "PS";

        public void SetNextIndex(string lastPersonId)
        {
            if (!string.IsNullOrEmpty(lastPersonId) && lastPersonId.StartsWith(_prefix))
            {
                var numberPart = lastPersonId.Substring(_prefix.Length);
                if (int.TryParse(numberPart, out int lastIndex))
                {
                    _nextIndex = lastIndex + 1;
                }
            }
            else
            {
                _nextIndex = 1;
            }
        }

        public string GenerateCode()
        {
            return $"{_prefix}{_nextIndex.ToString("D3")}";
        }
    }
}