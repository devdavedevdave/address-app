import Image from "next/image";
import { Button } from "@/components/ui/button"


export default function Home() {
  return (
    <main className="mt-28 container">
         <h1 className="scroll-m-20 text-4xl font-extrabold tracking-tight lg:text-5xl mb-8">Address Management</h1>
        <Button>Add new address</Button>
    </main>
  );
}
