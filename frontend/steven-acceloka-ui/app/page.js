"use client";

import { useEffect, useState } from "react";

export default function Home() {
  const [tickets, setTickets] = useState([]);
  const [page, setPage] = useState(1);
  const pageSize = 10;
  const [totalPages, setTotalPages] = useState(1);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [filters, setFilters] = useState({
    categoryName: "",
    ticketCode: "",
    ticketName: "",
    maxPrice: "",
    minEventDate: "",
    maxEventDate: "",
    orderBy: "ticketCode",
    orderState: "asc",
  });

  useEffect(() => {
    fetchTickets();
  }, [page, filters]);

  const fetchTickets = async () => {
    setLoading(true);
    setError(null);
    try {
      const queryParams = new URLSearchParams({
        page: page.toString(),
        pageSize: pageSize.toString(),
        ...Object.fromEntries(Object.entries(filters).filter(([_, v]) => v !== "")),
      });

      const response = await fetch(`http://localhost:5289/api/v1/get-available-ticket?${queryParams}`);
      const data = await response.json();

      setTickets(data.data || []);
      setTotalPages(data.totalPages || 1);
    } catch (error) {
      console.error("Error fetching tickets:", error);
      setError("Failed to fetch tickets.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container mx-auto p-6 bg-gray-100 rounded-2xl">
      <h1 className="text-2xl font-bold mb-4 text-center">Available Tickets</h1>

      {/* FILTER FORM */}
      <div className="bg-white p-4 rounded-lg shadow-md mb-6">
        <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
          <input
            type="text"
            placeholder="Category Name"
            value={filters.categoryName}
            onChange={(e) => setFilters({ ...filters, categoryName: e.target.value })}
            className="p-2 border rounded-md"
          />
          <input
            type="text"
            placeholder="Ticket Code"
            value={filters.ticketCode}
            onChange={(e) => setFilters({ ...filters, ticketCode: e.target.value })}
            className="p-2 border rounded-md"
          />
          <input
            type="text"
            placeholder="Ticket Name"
            value={filters.ticketName}
            onChange={(e) => setFilters({ ...filters, ticketName: e.target.value })}
            className="p-2 border rounded-md"
          />
          <input
            type="number"
            placeholder="Max Price"
            value={filters.maxPrice}
            onChange={(e) => setFilters({ ...filters, maxPrice: e.target.value })}
            className="p-2 border rounded-md"
          />
          <input
            type="date"
            value={filters.minEventDate}
            onChange={(e) => setFilters({ ...filters, minEventDate: e.target.value })}
            className="p-2 border rounded-md"
          />
          <input
            type="date"
            value={filters.maxEventDate}
            onChange={(e) => setFilters({ ...filters, maxEventDate: e.target.value })}
            className="p-2 border rounded-md"
          />
          <select
            value={filters.orderBy}
            onChange={(e) => setFilters({ ...filters, orderBy: e.target.value })}
            className="p-2 border rounded-md"
          >
            <option value="ticketName">Ticket Name</option>
            <option value="categoryName">Category Name</option>
            <option value="eventDate">Event Date</option>
            <option value="price">Price</option>
          </select>
          <select
            value={filters.orderState}
            onChange={(e) => setFilters({ ...filters, orderState: e.target.value })}
            className="p-2 border rounded-md"
          >
            <option value="asc">Ascending</option>
            <option value="desc">Descending</option>
          </select>
        </div>
        <button
          onClick={() => setPage(1)}
          className="mt-4 w-full bg-blue-600 text-white p-2 rounded-md hover:bg-blue-700"
        >
          Search
        </button>
      </div>

      {/* LOADING & ERROR MESSAGE */}
      {loading && <p className="text-center text-gray-700">Loading tickets...</p>}
      {error && <p className="text-center text-red-600">{error}</p>}

      {/* TICKET TABLE */}
      {!loading && !error && (
        <div className="overflow-x-auto">
          <table className="w-full table-auto border-collapse bg-white shadow-md rounded-lg overflow-hidden">
            <thead className="bg-gray-200">
              <tr>
                <th className="px-4 py-2">Event Date</th>
                <th className="px-4 py-2">Quota</th>
                <th className="px-4 py-2">Ticket Code</th>
                <th className="px-4 py-2">Ticket Name</th>
                <th className="px-4 py-2">Category</th>
                <th className="px-4 py-2">Price</th>
              </tr>
            </thead>
            <tbody>
              {tickets.map((ticket, index) => (
                <tr key={index} className="border-b hover:bg-gray-100">
                  <td className="px-4 py-2 text-center">{ticket.eventDate}</td>
                  <td className="px-4 py-2 text-center">{ticket.quota}</td>
                  <td className="px-4 py-2 text-center">{ticket.ticketCode}</td>
                  <td className="px-4 py-2 text-center">{ticket.ticketName}</td>
                  <td className="px-4 py-2 text-center">{ticket.categoryName}</td>
                  <td className="px-4 py-2 text-center">{ticket.price}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {/* PAGINATION */}
      <div className="mt-4 flex justify-between items-center">
        <button
          disabled={page === 1}
          onClick={() => setPage(page - 1)}
          className={`px-4 py-2 rounded-md ${page === 1 ? "bg-gray-300" : "bg-blue-600 text-white hover:bg-blue-700"}`}
        >
          Previous
        </button>
        <span className="text-gray-700">Page {page} of {totalPages}</span>
        <button
          disabled={page === totalPages}
          onClick={() => setPage(page + 1)}
          className={`px-4 py-2 rounded-md ${page === totalPages ? "bg-gray-300" : "bg-blue-600 text-white hover:bg-blue-700"}`}
        >
          Next
        </button>
      </div>
    </div>
  );
}
