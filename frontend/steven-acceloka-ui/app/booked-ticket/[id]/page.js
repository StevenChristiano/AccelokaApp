"use client";

import { useEffect, useState } from "react";
import { useParams } from "next/navigation";
import Link from "next/link";

export default function BookedTicketDetail() {
  const { id } = useParams(); // Get BookedTicketId from URL
  const [ticketDetails, setTicketDetails] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    if (!id) return; // Stop execution if id is missing

    const abortController = new AbortController();
    const signal = abortController.signal;

    const fetchBookedTicket = async () => {
      setLoading(true);
      setError(null);

      try {
        const response = await fetch(`http://localhost:5289/api/v1/get-booked-ticket/${id}`, { signal });

        if (!response.ok) {
          throw new Error(`Error ${response.status}: ${response.statusText}`);
        }

        const data = await response.json();

        if (!Array.isArray(data) || data.length === 0) {
          throw new Error("Data not found or incorrect format.");
        }

        setTicketDetails(data);
      } catch (err) {
        if (err.name !== "AbortError") {
          setError(err.message);
        }
      } finally {
        setLoading(false);
      }
    };

    fetchBookedTicket();

    return () => abortController.abort();
  }, [id]);

  if (loading)
    return <p className="text-center text-gray-500 animate-pulse">Loading...</p>;
  if (error)
    return <p className="text-center text-red-500 font-semibold">{error}</p>;

  return (
    <div className="p-6">
      <div className="w-full max-w-2xl mx-auto bg-white rounded-2xl shadow-xl p-6">
        
        {/* Tombol Back */}
        <div className="mb-4">
          <Link
            href="/booked-ticket"
            className="inline-flex items-center gap-2 text-gray-500 hover:text-blue-700 transition duration-200"
          >
            ⬅ Back
          </Link>
        </div>

        <h2 className="text-3xl font-bold text-center text-gray-800 mb-6">
          Booked Ticket Details
        </h2>

        {ticketDetails.length > 0 ? (
          <div className="space-y-6">
            {ticketDetails.map((category, categoryIndex) => (
              <div
                key={categoryIndex}
                className="p-5 scale-95 border rounded-xl shadow-md bg-gray-50 hover:scale-100 transition duration-200"
              >
                {/* Category Name */}
                <h3 className="text-xl font-semibold text-gray-700">
                  {category.categoryName}
                </h3>
                <p className="text-gray-600">
                  <strong>Quantity per Category:</strong> {category.qtyPerCategory}
                </p>

                {/* Tickets List */}
                <ul className="mt-4 space-y-3">
                  {category.tickets.map((ticket, ticketIndex) => (
                    <li
                      key={ticketIndex}
                      className="p-4 border rounded-lg scale-95 shadow-sm bg-white hover:bg-blue-50 hover:scale-100 transition duration-300"
                    >
                      <p className="text-gray-700">
                        <strong>🎟 Ticket Code:</strong> {ticket.ticketCode}
                      </p>
                      <p className="text-gray-700">
                        <strong>🏷 Ticket Name:</strong> {ticket.ticketName}
                      </p>
                      <p className="text-gray-700">
                        <strong>📅 Event Date:</strong>{" "}
                        {new Date(ticket.eventDate).toLocaleDateString("en-EN", {
                          day: "numeric",
                          month: "long",
                          year: "numeric",
                        })}{" "}
                        at{" "}
                        {new Date(ticket.eventDate).toLocaleTimeString([], {
                          hour: "2-digit",
                          minute: "2-digit",
                          hour12: false,
                        })}
                      </p>
                    </li>
                  ))}
                </ul>
              </div>
            ))}
          </div>
        ) : (
          <p className="text-center text-gray-500">No booked tickets found.</p>
        )}
      </div>
    </div>
  );
}
