"use client";

import { useState } from "react";
import { FiEdit3, FiHash, FiClipboard, FiLayers } from "react-icons/fi";

export default function EditBookedTicket() {
  const [bookedTicketId, setBookedTicketId] = useState("");
  const [ticketCode, setTicketCode] = useState("");
  const [newQty, setNewQty] = useState("");
  const [loading, setLoading] = useState(false);
  const [response, setResponse] = useState(null);
  const [error, setError] = useState(null);

  const handleEdit = async () => {
    setLoading(true);
    setError(null);
    setResponse(null);

    if (!bookedTicketId || !ticketCode || !newQty) {
      setError("All fields must be filled before updating.");
      setLoading(false);
      return;
    }

    if (parseInt(newQty) < 1) {
      setError("New ticket quantity must be at least 1.");
      setLoading(false);
      return;
    }

    const requestBody = {
      bookedTicketId: parseInt(bookedTicketId),
      tickets: [{ ticketCode, quantity: parseInt(newQty) }],
    };

    try {
      const res = await fetch(
        `http://localhost:5289/api/v1/edit-booked-ticket/${bookedTicketId}`,
        {
          method: "PUT",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(requestBody),
        }
      );

      const data = await res.json();
      if (!res.ok) throw new Error(data.detail || "Error updating ticket.");
      setResponse(data.tickets);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex items-center justify-center min-h-screen p-6">
      <div className="bg-white p-8 rounded-3xl shadow-2xl w-full max-w-lg transform hover:scale-105 transition-all">
        <h2 className="text-3xl font-extrabold text-gray-800 text-center mb-6 flex items-center justify-center gap-2">
          <FiEdit3 className="text-blue-600" /> Edit Booked Ticket
        </h2>

        <div className="space-y-4">
          {[{
            placeholder: "Booked Ticket ID",
            value: bookedTicketId,
            onChange: setBookedTicketId,
            icon: <FiHash className="text-gray-500" />,
          }, {
            placeholder: "Ticket Code",
            value: ticketCode,
            onChange: setTicketCode,
            icon: <FiClipboard className="text-gray-500" />,
          }, {
            placeholder: "New Quantity",
            value: newQty,
            onChange: setNewQty,
            icon: <FiLayers className="text-gray-500" />,
          }].map(({ placeholder, value, onChange, icon }, index) => (
            <div key={index} className="flex items-center gap-3 border rounded-xl p-3 bg-gray-50 shadow-sm transition-all focus-within:ring-2 focus-within:ring-blue-400">
              {icon}
              <input
                type={placeholder === "New Quantity" ? "number" : "text"}
                placeholder={placeholder}
                value={value}
                onChange={(e) => onChange(e.target.value)}
                className="w-full bg-transparent outline-none text-gray-700"
              />
            </div>
          ))}
        </div>

        <button
          onClick={handleEdit}
          className="w-full mt-6 py-3 bg-blue-600 text-white rounded-xl text-lg font-semibold shadow-md hover:bg-blue-700 transition-all disabled:opacity-50 disabled:cursor-not-allowed"
          disabled={loading}
        >
          {loading ? "Processing..." : "Update Ticket"}
        </button>

        {error && <p className="text-red-500 text-center mt-4">{error}</p>}

        {response && (
          <div className="mt-6 p-5 border rounded-2xl bg-gray-100 shadow-lg">
            <h3 className="text-xl font-bold text-gray-700 mb-3">Updated Booked Tickets</h3>
            {response.map((ticket, index) => (
              <div key={index} className="border p-4 rounded-xl bg-white shadow flex flex-col my-5">
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
                  <span className="font-semibold text-blue-600">Updated Quantity:</span> {ticket.remainingQuantity}
                </p>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}
