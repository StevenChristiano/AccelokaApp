"use client";

import { useState } from "react";
import { FiTrash2, FiHash, FiClipboard, FiLayers } from "react-icons/fi";

export default function RevokeTicket() {
  const [bookedTicketId, setBookedTicketId] = useState("");
  const [ticketCode, setTicketCode] = useState("");
  const [qty, setQty] = useState("");
  const [loading, setLoading] = useState(false);
  const [response, setResponse] = useState([]);
  const [error, setError] = useState(null);

  const handleRevoke = async () => {
    setLoading(true);
    setError(null);
    setResponse([]);

    if (!bookedTicketId || !ticketCode || !qty) {
      setError("All fields must be filled before revoking the ticket.");
      setLoading(false);
      return;
    }

    try {
      const res = await fetch(
        `http://localhost:5289/api/v1/revoke-ticket/${bookedTicketId}/${ticketCode}/${qty}`,
        {
          method: "DELETE",
          headers: {
            Accept: "application/json",
          },
        }
      );

      const data = await res.json();
      if (!res.ok) {
        throw new Error(data.detail || "An error occurred");
      }

      if (!Array.isArray(data.tickets)) {
        throw new Error("Invalid API response (expected an array)");
      }

      setResponse(data.tickets);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex items-center justify-center min-h-screen p-6">
      <div className="min-w-md mx-auto p-8 rounded-3xl shadow-2xl w-full max-w-lg bg-white  transition-all transform hover:scale-105">
        <h2 className="text-3xl font-extrabold text-gray-800 text-center mb-6 flex items-center justify-center gap-2">
          <FiTrash2 className="text-red-500"/> Revoke Ticket
        </h2>

        <div className="space-y-4 my-5">
          <div className="flex items-center border rounded-xl p-3 gap-3 bg-gray-50 shadow-sm transition-all focus-within:ring-2 focus-within:ring-blue-400">
            <FiHash className="text-gray-500 mr-2" />
            <input
              type="text"
              placeholder="Booked Ticket ID"
              value={bookedTicketId}
              onChange={(e) => setBookedTicketId(e.target.value)}
              className="w-full outline-none bg-transparent"
            />
          </div>

          <div className="flex items-center border rounded-xl p-3 gap-3 bg-gray-50 shadow-sm transition-all focus-within:ring-2 focus-within:ring-blue-400">
            <FiClipboard className="text-gray-500 mr-2" />
            <input
              type="text"
              placeholder="Ticket Code"
              value={ticketCode}
              onChange={(e) => setTicketCode(e.target.value)}
              className="w-full outline-none bg-transparent"
            />
          </div>

          <div className="flex items-center border rounded-xl p-3 gap-3 bg-gray-50 shadow-sm transition-all focus-within:ring-2 focus-within:ring-blue-400">
            <FiLayers className="text-gray-500 mr-2" />
            <input
              type="number"
              placeholder="Quantity"
              value={qty}
              onChange={(e) => setQty(e.target.value)}
              className="w-full outline-none bg-transparent"
            />
          </div>

          <button
            onClick={handleRevoke}
            className="w-full bg-red-600 text-white py-2 rounded-lg text-lg font-semibold shadow-md hover:bg-red-700 transition duration-200 disabled:opacity-50 mt-4"
            disabled={loading}
          >
            {loading ? "Processing..." : "Revoke Ticket"}
          </button>
        </div>

        {error && <p className="text-red-500 mt-3 text-sm">{error}</p>}

        {response.length > 0 && (
          <div className="mt-6 p-4 border rounded-lg bg-gray-50 shadow">
            <h3 className="text-lg font-semibold text-gray-700 mb-3">Remaining Tickets</h3>
            <div className="space-y-3">
              {response.map((ticket, index) => (
                <div key={index} className="border p-4 rounded-lg bg-white shadow flex flex-col">
                  <p className="text-gray-700 font-medium">
                    <span className="font-semibold text-blue-600">Code:</span> {ticket.ticketCode}
                  </p>
                  <p className="text-gray-700 font-medium">
                    <span className="font-semibold text-blue-600">Name:</span> {ticket.ticketName}
                  </p>
                  <p className="text-gray-700 font-medium">
                    <span className="font-semibold text-blue-600">Category:</span> {ticket.categoryName}
                  </p>
                  <p className="text-gray-700 font-medium">
                    <span className="font-semibold text-blue-600">Remaining Quantity:</span> {ticket.remainingQuantity}
                  </p>
                </div>
              ))}
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
