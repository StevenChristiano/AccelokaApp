
# Acceloka Ticket Booking System

This is an online ticket booking system using ASP.NET 8 and Next.js.

## Features
- View available tickets with filtering and sorting.
- Book tickets.
- View booked tickets.
- Cancel or edit booked tickets.

## Technologies Used
- Backend: ASP.NET 8, SQL Server/PostgreSQL, Entity Framework Core, MediatR.
- Frontend: Next.js (React), Tailwind CSS.
- Logging: Serilog.

## Setup Instructions
### 1. Clone the repository
### 2. Navigate to the backend folder and run:
- cd StevenAccelokaAPI
- dotnet restore
- dotnet run
### 3. Navigate to the frontend folder and run:
- cd ../frontend/steven-acceloka-ui
- npm install
- npm run dev

### BUG
There is a bug still with the Book Ticket. When I add the ticket with lowercase, it can't find the ticket, but the BookedTicketId still accept it, so when i want to book another ticket, the bookedTicketID skipped a number. The solution is maybe to add more validation to it. (Fixed)
