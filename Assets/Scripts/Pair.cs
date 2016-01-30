using System;
using System.Collections;
using System.Collections.Generic;

internal class Pair<A, B>
{
    private A _first;
    public A first
    {
        get
        {
            return _first;
        }
        set
        {
            _first = value;
        }
    }

    private B _second;
    public B second
    {
        get
        {
            return _second;
        }
        set
        {
            _second = value;
        }
    }
    

    public Pair(A a, B b)
    {
        this._first = a;
        this._second = b;
    }

    public Pair()
    {
        this._first = default(A);
        this._second = default(B);
    }
}