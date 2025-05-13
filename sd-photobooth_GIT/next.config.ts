import type { NextConfig } from "next";
import createNextIntlPlugin from 'next-intl/plugin';
import withBundleAnalyzer from '@next/bundle-analyzer';
const withNextIntl = createNextIntlPlugin();

const withAnalyzer = withBundleAnalyzer({
  enabled: process.env.ANALYZE === 'true',
});

const nextConfig: NextConfig = {
  images: {
    remotePatterns: [
      {
        protocol: "https",
        hostname: "firebasestorage.googleapis.com",
        pathname: "/v0/b/**", 
      },
      {
        protocol: "https",
        hostname: "imgcdn.stablediffusionweb.com",
        pathname: "/**",
      },
      {
        protocol: "https",
        hostname: "img.goodfon.com",
        pathname: "/**",
      },
      {
        protocol: "https",
        hostname: "image.civitai.com",
        pathname: "/**",
      },
      {
        protocol: "https",
        hostname: "cdn.prod.website-files.com",
        pathname: "/**",
      },
      {
        protocol: "https",
        hostname: "encrypted-tbn0.gstatic.com",
        pathname: "/**",
      },
      {
        protocol: "https",
        hostname: "cdn.pixeldojo.ai",
        pathname: "/**",
      },
      {
        protocol: "https",
        hostname: "mlf2vmoci1kz.i.optimole.com",
        pathname: "/**",
      },
      {
        protocol: "https",
        hostname: "i.etsystatic.com",
        pathname: "/**",
      },
      {
        protocol: "https",
        hostname: "www.prostarra.com",
        pathname: "/**",
      }
      
    ],
   
  },
 
  experimental: {
    optimizePackageImports:['@mantine/core', '@mantine/hooks', '@mantine/charts']
  },
  
};

export default withNextIntl(withAnalyzer(nextConfig));
