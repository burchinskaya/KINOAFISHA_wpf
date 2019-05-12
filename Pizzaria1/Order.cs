using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KINOwpf
{
    abstract class Order
    {
        public Order(int Range, int Place, int Code, int Cost)
        {
            this.Range = Range;
            this.Place = Place;
            this.Code = Code;
            this.Cost = Cost;
        }
        public int Cost { get; protected set; }
        public int Range { get; protected set; }
        public int Place { get; protected set; }
        public int Code { get; protected set; }
        public abstract int GetCost();
    }

    class SimpleOrder : Order
    {
        public SimpleOrder(int Range, int Place, int Code, int Cost) : base(Range, Place, Code, Cost)
        { }
        public override int GetCost()
        {
            return Cost;
        }
    }

    class StudentOrder : Order
    {
        public StudentOrder(int Range, int Place, int Code, int Cost) : base(Range, Place, Code, Cost)
        { }
        public override int GetCost()
        {
            return Cost - 10;
        }
    }

    class RetireeOrder : Order
    {
        public RetireeOrder(int Range, int Place, int Code, int Cost) : base(Range, Place, Code, Cost)
        { }
        public override int GetCost()
        {
            return Cost - 20;
        }
    }

    abstract class OrderDecorator : Order
    {
        protected Order order;
        public OrderDecorator(int Range, int Place, int Code, int Cost, Order order) : base(Range, Place, Code, Cost)
        {
            this.order = order;
        }
    }

    class MoreThan5Decorator : OrderDecorator
    {
        public MoreThan5Decorator(Order order)
            : base(order.Range, order.Place, order.Code, order.Cost, order)
        { }

        public override int GetCost()
        {
            return order.GetCost() - 5;
        }
    }

    class MoreThan10Decorator : OrderDecorator
    {
        public MoreThan10Decorator(Order order)
            : base(order.Range, order.Place, order.Code, order.Cost, order)
        { }

        public override int GetCost()
        {
            return order.GetCost() - 10;
        }
    }


}
