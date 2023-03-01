# Web Cryptography API testing application

At the moment, the tests are not very unit, as I am testing
as I implement the algorithms. It's more like a playground than
a formal testing.

The idea is to make this project evolve to be more robust
at testing every possible combination for the parameters.


## How to run the tests

Select this project as the startup project, and go to https://localhost:7114.
Then, there is two groups of tests:

1. Web Crypto API, which is the low-level interfaces implementations.
2. Cryptography, with higher level of abstraction.

When you click on one of these groups, the tests will then run.


## Cypress

I recently discovered [Cypress](https://www.cypress.io/), I will try
it to see if the testing can be automatized using it.

