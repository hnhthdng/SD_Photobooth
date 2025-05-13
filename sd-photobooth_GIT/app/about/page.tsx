import Link from "next/link";
import React from "react";

const AboutUs = () => {
  return (
    <div className="min-h-screen bg-gray-100 p-8">
      <div className="max-w-4xl mx-auto bg-white p-6 rounded-lg shadow-md">
        <h1 className="text-3xl font-bold text-center text-blue-600 mb-4">
          TechX SD Photobooth Dashboard
        </h1>

        <section className="mb-6">
          <h2 className="text-2xl font-semibold mb-2">Introduction</h2>
          <p className="text-gray-700">
            With advancements in AI and imaging technologies, the AI Photo Booth
            with Stable Diffusion aims to revolutionize the way people capture
            and share memories. By combining high-quality photo capture with
            AI-powered editing and customization, the system provides users with
            unique and creative experiences. Deployed in public spaces like
            malls and event venues, it serves as a tool for entertainment and
            marketing, engaging diverse audiences with its innovative features.
          </p>
        </section>

        <section className="mb-6">
          <h2 className="text-2xl font-semibold mb-2">Proposed Solutions</h2>
          <ul className="list-disc ml-6 text-gray-700">
            <li>Enhanced accessibility with strategically placed booths.</li>
            <li>
              AI-powered photo editing using Stable Diffusion for professional
              and creative results.
            </li>
            <li>Simple, user-friendly interfaces for seamless interactions.</li>
          </ul>
        </section>

        <section className="mb-6">
          <h2 className="text-2xl font-semibold mb-2">User Benefits</h2>
          <ul className="list-disc ml-6 text-gray-700">
            <li>
              High-quality photos with customizable AI-generated backgrounds.
            </li>
            <li>Instant sharing via social media or email.</li>
            <li>
              Community-building through interactive events and experiences.
            </li>
          </ul>
        </section>

        <section className="mb-6">
          <h2 className="text-2xl font-semibold mb-2">
            Functional Requirements
          </h2>

          <h3 className="text-xl font-semibold mt-4">Admin </h3>
          <ul className="list-disc ml-6 text-gray-700">
            <li>Manage employees, customize themes, and handle payments.</li>
            <li>Generate and manage session codes.</li>
            <li>Configure camera hardware.</li>
          </ul>

          <h3 className="text-xl font-semibold mt-4">Manager </h3>
          <ul className="list-disc ml-6 text-gray-700">
            <li>Manage staff and customer accounts.</li>
            <li>Monitor system performance and generate reports.</li>
            <li>Oversee operational locations and booth activities.</li>
            <li>Process orders and manage promotional discounts.</li>
          </ul>

          <h3 className="text-xl font-semibold mt-4">Staff </h3>
          <ul className="list-disc ml-6 text-gray-700">
            <li>Manage customer accounts and session codes.</li>
            <li>Track and process customer orders.</li>
            <li>Store and organize past photos for support.</li>
            <li>Assist customers and improve service quality.</li>
          </ul>
        </section>

        <footer className="mt-4 text-center text-sm text-gray-600 ">
          <Link href="/privacy" className=" hover:underline text-gray-800 mx-2">
            Privacy Policy
          </Link>
        </footer>
      </div>
    </div>
  );
};

export default AboutUs;
