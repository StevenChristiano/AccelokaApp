export async function getAvailableTickets(params) {
  const query = new URLSearchParams(params).toString();
  const res = await fetch(`http://localhost:5000/api/v1/get-available-ticket?${query}`);
  if (!res.ok) throw new Error('Failed to fetch data');
  return res.json();
}
