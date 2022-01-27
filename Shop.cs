using System;
using System.Collections.Generic;

struct Good
{
    public readonly string Name;

    public Good(string name)
    {
        Name = name;
    }
}

class GoodsContainer
{
    private Dictionary<Good, int> _content;

    public GoodsContainer()
    {
        Clear();
    }

    public void PutGoods(Good good, int count)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException("Negative count value");

        if (_content.ContainsKey(good) == false)
            _content[good] = count;
        else
            _content[good] += count;
    }

    public void TakeGoods(Good good, int count)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException("Negative count value");
        else if (_content.ContainsKey(good) == false || _content[good] < count)
            throw new ArgumentOutOfRangeException("Not enough instances");

        _content[good] -= count;
    }

    public Dictionary<Good, int> GetAllGoods()
    {
        Dictionary<Good, int> duplicate = new Dictionary<Good, int>();

        foreach(var key in _content.Keys)
        {
            duplicate[key] = _content[key];
        }

        return duplicate;
    }

    public void Clear()
    {
        _content = new Dictionary<Good, int>();
    }
}

class Warehouse
{
    private GoodsContainer _container;

    public Warehouse()
    {
        _container = new GoodsContainer();
    }

    public void Delive(Good good, int count)
    {
        _container.PutGoods(good, count);
    }

    public void Remove(Good good, int count)
    {
        _container.TakeGoods(good, count);
    }
}

class Shop
{
    private Warehouse _warehouse;

    public Shop(Warehouse warehouse)
    {
        _warehouse = warehouse;
    }

    public Cart Cart()
    {
        return new Cart(_warehouse);
    }
}

class Cart
{
    private GoodsContainer _container;
    private Warehouse _warehouse;

    public Cart(Warehouse warehouse)
    {
        _container = new GoodsContainer();
        _warehouse = warehouse;
    }

    public void Add(Good good, int count)
    {
        _warehouse.Remove(good, count);
        _container.PutGoods(good, count);
    }

    public Order Order()
    {
        Order newOrder =  new Order(_container.GetAllGoods());
        _container.Clear();
        return newOrder;
    }
}

class Order
{
    private GoodsContainer _container;
    public readonly string Paylink;

    public Order(Dictionary<Good, int> content)
    {
        _container = new GoodsContainer();
        Paylink = "";

        foreach(var key in content.Keys)
        {
            _container.PutGoods(key, content[key]);
            Paylink += $"{key.Name} ({content[key]}) \n";
        }
    }
}

class Program
{
    static void Main()
    {
        Good iPhone12 = new Good("IPhone 12");
        Good iPhone11 = new Good("IPhone 11");

        Warehouse warehouse = new Warehouse();

        Shop shop = new Shop(warehouse);

        warehouse.Delive(iPhone12, 10);
        warehouse.Delive(iPhone11, 1);

        //Вывод всех товаров на складе с их остатком

        Cart cart = shop.Cart();
        cart.Add(iPhone12, 4);
        cart.Add(iPhone11, 3); //при такой ситуации возникает ошибка так, как нет нужного количества товара на складе

        //Вывод всех товаров в корзине

        Console.WriteLine(cart.Order().Paylink);

        cart.Add(iPhone12, 9); //Ошибка, после заказа со склада убираются заказанные товары
    }

}