# C-coffeeshop

![demo](docs/demo.gif)

# Services

No. | Service | Uri
--- | --- | --- |
1 | Web | localhost:5150
2 | Order | localhost:5150 (Customers)
3 | Kitchen | localhost:5150/orders (Workers only)

The Kitchen can be ported to seperate service cause I use message queue to listen order submit. This is just a sample so I put it together.
