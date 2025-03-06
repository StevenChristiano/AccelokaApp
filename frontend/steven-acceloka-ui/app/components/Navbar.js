"use client"
import Link from 'next/link';

const Navbar = () => {
  return (
    <nav className="flex items-center justify-between px-6 py-8 bg-gradient-to-r from-gray-900 to-gray-700 shadow-lg">
      <div className="text-3xl font-bold text-white">Acceloka</div>
      <div className="flex gap-6">
        <Link href="/" className="text-white text-xl px-2 hover:text-gray-300 transition duration-300">Available Ticket</Link>
        <Link href="/booked-ticket" className="text-white text-xl px-2 hover:text-gray-300 transition duration-300">Booked Ticket</Link>
        <Link href="/book-ticket" className="text-white text-xl px-2 hover:text-gray-300 transition duration-300">Book Ticket</Link>
        <Link href="/edit-booked-ticket" className="text-white text-xl px-2 hover:text-gray-300 transition duration-300">Edit Booked Ticket</Link>
        <Link href="/revoke-ticket" className="text-white text-xl px-2 hover:text-gray-300 transition duration-300">Revoke Ticket</Link>
      </div>
    </nav>
  );
};

export default Navbar;
