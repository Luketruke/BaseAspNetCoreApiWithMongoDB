# 🚀 ASP.NET Core Base API – SOLID Architecture + Authentication + Chat + MongoDB  

This is a **ASP.NET Core** API boilerplate built with **SOLID principles**, providing a robust foundation for scalable and well-structured applications. It includes authentication, validation, error handling, real-time chat integration, and more.  

---

## 📌 **Main Features**  

✅ **SOLID Architecture** with clear layer separation:  
- **API** (Controllers)  
- **Application** (Services, DTOs, Validations)  
- **Domain** (Entities and business logic)  
- **Infrastructure** (Data access, authentication, integrations)  

✅ **Implemented Design Patterns**  
- **Repository Pattern** with `BaseRepository<T>`  
- **Unit of Work**  
- **Dependency Injection** throughout the architecture  

✅ **Authentication & Security**  
- **JWT Service** for session retrieval  
- **Login with Facebook, Google, and Email**  
- **Swagger** with authentication support  

✅ **Validation & Error Handling**  
- **FluentValidation** for data validation  
- **Middleware** with custom exception handling  

✅ **Database**  
- **MongoDB** as the primary database  
- **Entities defined in Domain, DTOs separated into Requests and Responses**  

✅ **Real-time Chat System**  
- **Pusher Integration** (Controller and classes ready for frontend implementation)  

✅ **Other Features**  
- **PasswordHasher** included in Infrastructure  
- **Swagger** configured for API documentation  

## 🛠 **Technologies Used**  
- **ASP.NET Core**  
- **MongoDB**  
- **JWT Authentication**  
- **FluentValidation**  
- **Pusher** (for real-time chat)  
- **Swagger**  

---
