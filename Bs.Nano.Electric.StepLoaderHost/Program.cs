using System.Text.Json;

namespace Bs.Nano.Electric.StepLoaderHost {

    class Program {
        static int Main() {
            try {
                var input = Console.ReadLine();
                if (input == null)
                    return -1;

                var args = JsonSerializer.Deserialize<Dictionary<string, string>>(input)
                           ?? new();

                var loader = new StepLoader();
                var id = loader.LoadGraphic(
                    args["Code"],
                    args["Category"],
                    args["GraphicName"],
                    args["strConnection"],
                    args["StepFileName"],
                    args["GraphicPath"],
                    args["CadWisePath"]
                );

                var result = new { Id = id, Error = (string?)null };
                Console.WriteLine(JsonSerializer.Serialize(result));
                return 0;
            }
            catch (Exception ex) {
                var result = new { Id = (int?)null, Error = ex.ToString() };
                Console.WriteLine(JsonSerializer.Serialize(result));
                return 1;
            }
        }
    }

}
