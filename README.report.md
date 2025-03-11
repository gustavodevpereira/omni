## This document aims to present my decisions throughout the project and my personal perspective on the challenge.

### 1. Domain Modeling  

When I read the documentation in the README, I immediately realized that I needed a `Sale` aggregate.  
At that point, the `Sale` aggregate required a subordinate entity, the `SaleItem`.  

Since the discount logic was closely related to the `SaleItem`, I decided to encapsulate it within a value object (`DiscountPolicy`). This approach allowed me to unit-test it early and decouple it from other entities.  

Later, I recognized the need for a `Product` aggregate to make the system functional. How else could I list all the available products for the user? So, I created a `Product` aggregate, keeping it simple since it was not the focus of the challenge.  

As the project progressed, I realized that `Sale` was not a descriptive name based on the API requirements.  
I refactored it to `Cart` and `CartItem`, which improved code readability and better represented its purpose.  

You can check the tests to see how I implemented the domain logic—I aimed to make them as readable as possible.  

### 2. Application Layer  

Early on, I noticed that the project was already designed to use the Mediator pattern for handling requests.  
I leveraged this existing design, making the implementation straightforward.  

#### **The Domain Events Problem**  

As required by the challenge, I had to implement domain events. My approach was to use a base class to absorb domain events within the domain layer.  
Every action in the domain that required an event would instantiate it in the base class, persisting it in a domain events list.  

However, that alone was not enough. I also implemented a `UnitOfWork` to handle `SaveChanges` when the ORM attempted to save an entity. The `UnitOfWork` ensured that domain events were delivered to the Mediator.  

All domain events are handled by the application layer. Whenever a domain event occurs, a handler forwards it to the `MessageBus` abstraction (implemented via RabbitMQ with Rebus), publishing it to a topic in our queue.  

You can verify this by creating a `Cart`—this action will be posted to the RabbitMQ queue through this workflow (along with other events).  

You can also check the tests for further details.  

### 3. API Layer  

The API layer was straightforward—I followed the design patterns already implemented in the project.  

### 4. Frontend  

The frontend was more challenging than I anticipated.  
The main issue I encountered was preventing users from creating a new cart in the backend on every sale attempt.  
Allowing unrestricted cart creation would open the system to abuse—imagine a user performing a DDoS attack simply by repeatedly creating carts without making a purchase.  

To solve this, I decided **not** to persist the cart in the database. Instead, it is stored locally on the frontend, and only when the user proceeds to checkout is the cart instantiated in the backend.  

Another challenge was applying the domain logic for the discount policy. Since the cart wasn't persisted, how could the user execute the discount logic?  
The solution was to send the cart's ViewModel to the server, where it would be instantiated in memory. The server would then apply the domain logic and return the updated cart with the discount applied.  

With that in place, no further issues arose on the client side.
