"use client";
import { useState, useEffect } from "react";
import Link from "next/link";
import styles from "../components/BookedTIcket.module.css";

export default function BookedTicketPage() {
  const [bookedIds, setBookedIds] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchBookedIds = async () => {
      try {
        const res = await fetch("http://localhost:5289/api/v1/get-all-booked-ids");
        const data = await res.json();
        setBookedIds(data);
      } catch (error) {
        console.error("Error fetching booked ticket IDs:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchBookedIds();
  }, []);

  return (
    <div className="flex items-center justify-center min-h-screen p-6">
      <div className="w-full max-w-3xl bg-white rounded-2xl shadow-2xl p-6">
        <h1 className="text-center text-3xl font-extrabold text-gray-800 mb-6">
          🎟 Booked Tickets
        </h1>

        {loading ? (
          <p className="text-center text-gray-500 animate-pulse">Loading...</p>
        ) : bookedIds.length === 0 ? (
          <p className="text-center text-gray-500">No booked tickets found.</p>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            {bookedIds.map((id) => (
                <Link key={id} href={`/booked-ticket/${id}`} className="block text-lg">
                  <div className={`${styles.ticket} hover:scale-105 hover:text-white transition-all duration-300 p-4 text-center`}>
                      🎫 Ticket ID: {id}
                  </div>
                </Link>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}
