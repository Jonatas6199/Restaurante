using Restaurante.Enumeradores;

namespace Restaurante.Entidades
{
    public class Pedido
    {
        public string Item { get; set; }
        public int NumeroPedido { get; set; }
        public StatusPedidoEnum StatusPedido { get; set; }
    }
}
