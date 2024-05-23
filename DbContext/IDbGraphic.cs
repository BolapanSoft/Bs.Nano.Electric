namespace Nano.Electric {
    public interface IDbGraphic : IHaveId {
        bool? AutoSelectSize { get; set; }
        string Category { get; set; }
        byte[] GraphicInBytes { get; set; }
        string Name { get; set; }
    }
}