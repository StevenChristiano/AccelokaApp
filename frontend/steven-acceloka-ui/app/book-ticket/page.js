"use client";

import { useState } from "react";
import { FiClipboard, FiPlus, FiTrash2 } from "react-icons/fi";

export default function BookTicket() {
  const [tickets, setTickets] = useState([{ ticketCode: "", quantity: 1 }]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const addTicket = () => setTickets([...tickets, { ticketCode: "", quantity: 1 }]);

  const removeTicket = (index) => {
    setTickets(tickets.filter((_, i) => i !== index));
  };

  const handleInputChange = (index, field, value) => {
    const updatedTickets = [...tickets];
    updatedTickets[index][field] = value;
    setTickets(updatedTickets);
  };

  const bookTickets = async () => {
    setLoading(true);
    setError("");
    setSuccess("");

    try {
      const response = await fetch("http://localhost:5289/api/v1/book-ticket", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ tickets }),
      });

      const data = await response.json();
      if (!response.ok) throw new Error(data.detail || "Booking failed");

      setSuccess("🎉 Booking successful!");
      setTickets([{ ticketCode: "", quantity: 1 }]);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex justify-center items-center min-h-screen bg-gradient-to-br p-4">
      <div className="w-full max-w-lg p-8 bg-white rounded-2xl shadow-xl transition-all transform hover:scale-105">
        <h2 className="text-3xl font-extrabold text-gray-800 text-center mb-6">🎟️ Book Tickets</h2>

        {/* Dynamic Form */}
        <div className="space-y-4">
          {tickets.map((ticket, index) => (
            <div key={index} className="flex items-center gap-3 p-3 bg-gray-50 border rounded-lg shadow-sm transition transform hover:shadow-md">
              <FiClipboard className="text-gray-500 text-xl" />
              <input
                type="text"
                placeholder="Ticket Code"
                value={ticket.ticketCode.toUpperCase()}
                onChange={(e) => handleInputChange(index, "ticketCode", e.target.value)}
                className="flex-1 p-2 rounded-lg border focus:ring-2 focus:ring-blue-400 focus:outline-none transition"
              />
              <input
                type="number"
                min="1"
                value={ticket.quantity}
                onChange={(e) => handleInputChange(index, "quantity", e.target.value)}
                className="w-20 p-2 border rounded-lg text-center focus:ring-2 focus:ring-blue-400 focus:outline-none transition"
              />
              {index > 0 && (
                <button
                  onClick={() => removeTicket(index)}
                  className="p-2 bg-red-500 hover:bg-red-600 text-white rounded-lg shadow-md transition transform hover:scale-110"
                >
                  <FiTrash2 />
                </button>
              )}
            </div>
          ))}
        </div>

        {/* Add Ticket Button */}
        <button
          onClick={addTicket}
          className="w-full mt-4 flex items-center justify-center gap-2 bg-blue-500 hover:bg-blue-600 text-white py-3 rounded-lg shadow-md transition transform hover:scale-105"
        >
          <FiPlus />
          Add Ticket
        </button>

        {/* Submit Button */}
        <button
          onClick={bookTickets}
          disabled={loading}
          className={`w-full mt-4 py-3 rounded-lg text-white shadow-md transition transform hover:scale-105 ${
            loading ? "bg-gray-400 cursor-not-allowed" : "bg-green-500 hover:bg-green-600"
          }`}
        >
          {loading ? "Booking..." : "Book Now"}
        </button>

        {/* Notification Messages */}
        {error && <p className="text-red-500 text-center mt-4">{error}</p>}
        {success && <p className="text-green-500 text-center mt-4">{success}</p>}
      </div>
    </div>
  );
}
