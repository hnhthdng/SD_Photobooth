import React from "react";
import { Button, Container, SimpleGrid, Text, Title } from "@mantine/core";
// import image from './image.svg';
const FailedPage = () => {
  return (
    <Container className="py-20">
      <SimpleGrid
        spacing={{ base: 40, sm: 80 }}
        cols={{ base: 1, sm: 2 }}
        className="items-center"
      >
        {/* Ảnh trên mobile */}
        {/* <Image src={image.src} className="block sm:hidden" /> */}
        <div className="text-center sm:text-left">
          <Title className="text-3xl font-extrabold text-red-600">
            Something went wrong...
          </Title>
          <Text
            c="dimmed"
            size="lg"
            className="max-w-lg mx-auto sm:mx-0 mt-4 text-gray-500"
          >
            Your payment could not be processed. You may have canceled the
            transaction, or an error occurred. If you need help, please contact
            support.
          </Text>
          
        </div>
       
      </SimpleGrid>
    </Container>
  );
};

export default FailedPage;
