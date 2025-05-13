"use client";

export default function PrivacyPolicy() {
  return (
    <div className="max-w-3xl mx-auto p-6">
      <h1 className="text-3xl font-bold mb-4">Privacy Policy</h1>
      <p className="text-gray-700 mb-4">Last Updated: 27/02/2025</p>

      <p className="mb-4">
        Welcome to AI-PhotoBooth Dashboard. We value your privacy and are
        committed to protecting your personal data. This Privacy Policy explains
        how we collect, use, store, and share your information, including data
        from Google services.
      </p>

      <h2 className="text-2xl font-semibold my-3">1. Information We Collect</h2>
      <ul className="list-disc pl-6 mb-4">
        <li>Personal information you provide (e.g., name, email).</li>
        <li>Usage data (e.g., pages visited, time spent on our site).</li>
        <li>Google user data if you authenticate with Google services.</li>
      </ul>

      <h2 className="text-2xl font-semibold my-2">
        2. How We Use Your Information
      </h2>
      {/* <p className="mb-4">We use collected data to:</p> */}
      <ul className="list-disc pl-6 my-2">
        <li>Provide and improve our services.</li>
        <li>Personalize your experience.</li>
        <li>Communicate with you (e.g., support, updates).</li>
        <li>Ensure compliance with our policies and legal requirements.</li>
      </ul>

      <h2 className="text-2xl font-semibold my-3">3. Google User Data</h2>
      <p className="mb-4">
        If you sign in using Google, we access your Google profile (name, email)
        to create and manage your account. We do not store or share this data
        beyond its intended purpose.
      </p>

      <h2 className="text-2xl font-semibold my-3">
        4. Data Sharing & Security
      </h2>
      <p className="mb-4">
        We do not sell your personal data. Your information is only shared with
        trusted third parties as necessary (e.g., payment processors,
        authentication services) while ensuring data protection.
      </p>

      <h2 className="text-2xl font-semibold my-3">5. Changes to This Policy</h2>
      <p className="mb-4">
        We may update this Privacy Policy. Significant changes will be notified
        through our website or via email.
      </p>

      <h2 className="text-2xl font-semibold my-3">6. Contact Us</h2>
      <p className="mb-4">
        If you have any questions, please contact us at:{" "}
        <a href="mailto:support@yourdomain.com" className="text-blue-600">
          support@yourdomain.com
        </a>
      </p>
    </div>
  );
}
