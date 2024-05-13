namespace Nano.Electric {
    public interface IDbGraphic {
        bool? AutoSelectSize { get; set; }
        string Category { get; set; }
        byte[] GraphicInBytes { get; set; }
        string Name { get; set; }
    }
}