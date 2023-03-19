using System.Threading.Tasks;

namespace MyApp
{
    public class MyClass
    {
        public async Task<int> GetAnswerAsync()
        {
            // Some async code here...
            await Task.Delay(1000);
            return 42;
        }
    }
}