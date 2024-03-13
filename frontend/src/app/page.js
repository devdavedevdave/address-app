"use client";

import { useEffect, useState } from 'react';
import { DataTable } from "@/components/ui/data-table";
import { columns } from "@/components/ui/columns";

export default function Home() {
  const [data, setData] = useState([]);
  const [form, setForm] = useState({
    id: 0,
    street: '',
    city: '',
    state: '',
    postalCode: '',
    country: '',
  });

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const response = await fetch('https://localhost:7109/address');
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
      const data = await response.json();
      setData(data);
    } catch (error) {
      console.error('There was a problem with your fetch operation:', error);
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm(prevForm => ({ ...prevForm, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const method = form.id === 0 ? 'POST' : 'PUT';
    const url = form.id === 0 ? 'https://localhost:7109/address' : `https://localhost:7109/address/${form.id}`;

    try {
      const response = await fetch(url, {
        method: method,
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(form),
      });

      if (!response.ok) {
        throw new Error('Failed to save data');
      }

      setForm({
        id: 0,
        street: '',
        city: '',
        state: '',
        postalCode: '',
        country: '',
      });
      fetchData(); // Refresh the data
    } catch (error) {
      console.error('Error saving data:', error);
    }
  };

  // This function is triggered when you want to edit an address
  const handleEdit = (address) => {
    setForm(address);
  };

  return (
    <main className="mt-28 container">
      <h1 className="scroll-m-20 text-4xl font-extrabold tracking-tight lg:text-5xl mb-8">Address Management</h1>
      <DataTable columns={columns} data={data} onEdit={handleEdit} />
      <div className="h-5"></div>
      <h2 className="mb-2">{form.id === 0 ? 'Add New Address' : 'Edit Address'}</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <input type="text" name="street" placeholder="Street" value={form.street} onChange={handleChange} required />
        </div>
        <div>
          <input type="text" name="city" placeholder="City" value={form.city} onChange={handleChange} required />
        </div>
        <div>
          <input type="text" name="state" placeholder="State" value={form.state} onChange={handleChange} required />
        </div>
        <div>
          <input type="text" name="postalCode" placeholder="Postal Code" value={form.postalCode} onChange={handleChange} required />
        </div>
        <div>
          <input type="text" name="country" placeholder="Country" value={form.country} onChange={handleChange} required />
        </div>
        <div>
          <button type="submit" className="Button component class here">Submit</button>
        </div>
      </form>
    </main>
  );
}
