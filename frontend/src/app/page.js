"use client";

import { useEffect, useState } from 'react';
import { Button } from "@/components/ui/button";
import { DataTable } from "@/components/ui/data-table";
import { columns } from "@/components/ui/columns";

export default function Home() {
  const [data, setData] = useState([]);

  useEffect(() => {
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

    fetchData();
  }, []);

  return (
    <main className="mt-28 container">
         <h1 className="scroll-m-20 text-4xl font-extrabold tracking-tight lg:text-5xl mb-8">Address Management</h1>
        <Button>Add new address</Button>
        <div className="h-10"></div>
        <DataTable columns={columns} data={data} />
    </main>
  );
}
