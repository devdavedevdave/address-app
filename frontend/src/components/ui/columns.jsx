"use client";

import { ColumnDef } from "@tanstack/react-table";

// You can still document the shape of your data in comments if desired
// This is the shape of our data for payments
export const columns = [
  {
    accessorKey: "street",
    header: "Street",
  },
  {
    accessorKey: "city",
    header: "City",
  },
  {
    accessorKey: "state",
    header: "State",
  },
  {
    accessorKey: "postalCode",
    header: "PostalCode",
  },
  {
    accessorKey: "country",
    header: "Country",
  },
];
